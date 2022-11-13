using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CHANDsPAPERService.Models
{
    public class HomeModel
    {
        [Required(ErrorMessage = "Please select a file")]
        public IFormFile UploadFIle { set; get; }
        public string Filepath { set; get; }
        public int MentorID { set; get; }
        public string filetype { set; get; }

        public List<ListFile> extractfiles { set; get; }

        public List<LocalfilesModel> GetLocalImages { set; get; }

    }

    public class ListFile
    {
        public string Filepath { set; get; }
        public string FileuniqueID { set; get; }
    }

    public class LocalfilesModel
    {
        public int FileuniqueID { set; get; }
        public string FileName { set; get; }
        public DateTime CreatedDate { set; get; }
        public string Fileuniquepath { set; get; }
        public bool isdeleted { set; get; }
        public bool islive { set; get; }
        public int TotalrowCount { set; get; }
        public string uploadeddate { set; get; }
    }

    public class ExceptionModel
    {

    }
    public class TextContentSendModel
    {
        public int textcontentid{set;get;}
        public string textcontent{set;get;}
        public string CaptionforTextContent {set;get;}
        public string TitleforTextContent {set;get;}
        public string PostType { set; get; }
        public int MentorID { set; get; }

        [Required(ErrorMessage ="Please enter subject")]
        public string Subject { set; get; }
    }
}

