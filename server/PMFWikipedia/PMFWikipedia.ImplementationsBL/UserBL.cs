using AutoMapper;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using System.Text.RegularExpressions;

namespace PMFWikipedia.ImplementationsBL
{
    public class UserBL : IUserBL
    {
        public readonly IUserDAL _userDAL;
        private readonly IMapper _mapper;
        
        public UserBL(IUserDAL userDAL, IMapper mapper)
        {
            _userDAL = userDAL;
            _mapper = mapper;
        }
        public async Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo)
        {
            string pattern = @"@pmf\.kg\.ac\.rs$";

            if (await _userDAL.CheckEmail(registerInfo.Email))
            {
                return new ActionResultResponse<User>(null, false, "Email Taken");
            }
            else if (!Regex.IsMatch(registerInfo.Email, pattern))
            {
                return new ActionResultResponse<User>(null, false, "Email Invalid");
            }

            User newUser = _mapper.Map<User>(registerInfo);
            newUser.Password = PasswordService.HashPass(registerInfo.Password);

            await _userDAL.Insert(newUser);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<User>(newUser, true, "");

        }
    }
}