using AutoMapper;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
using System;

namespace PMFWikipedia.ImplementationsBL
{
    public class ChatBL : IChatBL
    {
        private readonly IChatDAL _chatDAL;
        private readonly IMessageDAL _messageDAL;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;
        private readonly IUserDAL _userDAL;
        public ChatBL(IChatDAL chatDAL, IMessageDAL messageDAL, IMapper mapper, IJWTService jwtservice, IUserDAL userDAL)
        {
            _chatDAL = chatDAL;
            _messageDAL = messageDAL;
            _mapper = mapper;
            _jwtService = jwtservice;
            _userDAL = userDAL;
        }

        public async Task<ActionResultResponse<List<ChatInfo>>> GetAllChats(long id)
        {
            var chats = await _chatDAL.GetChatsById(id);

            List<ChatInfo> chatInfos = new List<ChatInfo>();
            foreach(var chat in chats)
            {
                var c = new ChatInfo();
                c.Id = chat.Id;
                c.TimeStamp = chat.DateModified;
                if (chat.User1 != null && chat.User2 != null)
                {
                    c.User1Id = (long)chat.User1;
                    c.User2Id = (long)chat.User2;
                }

                if (chat.User1Navigation.Id != id)
                    c.User = _mapper.Map<UserViewModel>(chat.User1Navigation);
                else
                    c.User = _mapper.Map<UserViewModel>(chat.User2Navigation);

                c.Unread = chat.Messages.Count;
                chatInfos.Add(c);
            }
            return new ActionResultResponse<List<ChatInfo>>(chatInfos, true, "");
        }
        public async Task<ActionResultResponse<int>> GetUnreadMessages(long id)
        {
            var chats = await _chatDAL.GetNumberOfUnreadMessages(id);
            var number = 0;
            if (chats != null && chats.Count > 0)
            {
                foreach (var chat in chats)
                {
                    number += chat.Messages.Count;
                }
            }

            return new ActionResultResponse<int>(number, true, "");
        }

        public async Task<ActionResultResponse<List<ChatViewModel>>> GetMessages(long id)
        {
            //URADITI PAGGING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var chat = await _chatDAL.GetChatsById(id);
            if(chat == null)
            {
                return new ActionResultResponse<List<ChatViewModel>>(null, false, "Chat doesnt exist");
            }

            List<Message> messages = new List<Message>();

            messages = await _messageDAL.GetMessagesByChatId(id);

            List<ChatViewModel> chatViewModel = new List<ChatViewModel>();
            foreach (var m in messages)
            {
                var c = _mapper.Map<ChatViewModel>(m);
                chatViewModel.Add(c);
            }
            return new ActionResultResponse<List<ChatViewModel>>(chatViewModel, true, "");
        }
        public async Task<ActionResultResponse<ChatViewModel>> InsertMessage(long user1Id, long user2Id, string message)
        {
            ChatViewModel cvm = new ChatViewModel();

            Chat chat =  await _chatDAL.GetChatId(user1Id, user2Id);


            //MAPIRAJ OVO
            if (chat == null)
            {
                Chat c = new Chat();
                c.User1 = user1Id;
                c.User2 = user2Id;
                await _chatDAL.Insert(c);
                await _chatDAL.SaveChangesAsync();

                Message m = new Message();
                m.Content = message;
                m.TimeStamp = DateTime.Now;
                m.IsRead = false;
                m.ChatId = c.Id;
                m.SenderId = user1Id;
                await _messageDAL.Insert(m);
                await _messageDAL.SaveChangesAsync();

                User u = await _userDAL.GetById(user1Id);
                var userReturn = _mapper.Map<UserViewModel>(u);

                cvm.User1Id = user1Id;
                cvm.User2Id = user2Id;
                cvm.User = userReturn;
                cvm.Id = m.Id;
                cvm.SenderId = user1Id;
                cvm.ChatId = c.Id;
                cvm.Content = message;
                cvm.IsRead = false;
                cvm.TimeStamp = DateTime.Now;
            }

            else 
            {
                Message m = new Message();
                m.Content = message;
                m.TimeStamp = DateTime.Now;
                m.IsRead = false;
                m.ChatId = chat.Id;
                m.SenderId = user1Id;
                chat.DateModified = DateTime.Now;

                await _chatDAL.SaveChangesAsync();
                await _messageDAL.Insert(m);
                await _messageDAL.SaveChangesAsync();

                User u = await _userDAL.GetById(user1Id);
                var userReturn = _mapper.Map<UserViewModel>(u);

                cvm.User1Id = user1Id;
                cvm.User2Id = user2Id;
                cvm.User = userReturn;
                cvm.SenderId = user1Id;
                cvm.ChatId = chat.Id;
                cvm.Content = message;
                cvm.IsRead = false;
                cvm.TimeStamp = DateTime.Now;
                cvm.Id = m.Id;
            }
            return new ActionResultResponse<ChatViewModel>(cvm, true, "");
        }
    }
}