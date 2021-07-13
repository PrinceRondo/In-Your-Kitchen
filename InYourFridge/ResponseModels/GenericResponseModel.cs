﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GenericResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
        public IList<object> Datas { get; set; }
    }
}
