﻿using OnionApiUpgradeBogus.WebApi.Models;

namespace OnionApiUpgradeBogus.WebApi.ViewModels
{
    public class SearchViewModel
    {
        public string Term { get; set; }
        public List<Book> Results { get; set; }
    }
}
