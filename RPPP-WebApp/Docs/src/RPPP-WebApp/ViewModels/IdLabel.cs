
using System.Text.Json.Serialization;

 

﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace RPPP_WebApp.ViewModels
{


    /// <summary>
    /// Model za autocomplete
    /// </summary>
    public class IdLabel
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public IdLabel() { }
        public IdLabel(int id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}