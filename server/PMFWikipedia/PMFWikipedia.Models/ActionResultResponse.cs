using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMFWikipedia.Models
{
    public class ActionResultResponse<T>
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public bool Status { get; set; }

        public ActionResultResponse() 
        {
            Errors = new List<string>();
        }

        public ActionResultResponse(T? data, bool status, string error) 
        {
            Data = data;
            Errors = new List<string>();
            Status = status;
            Errors.Add(error);
        }
    }
}
