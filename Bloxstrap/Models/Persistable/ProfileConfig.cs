using System;
using System.Collections.Generic;

namespace Bloxstrap.Models.Persistable
{
    public class ProfileConfig
    {
        public string Active { get; set; } = "default";

        public List<string> Profiles { get; set; } = new() { "default" };
    }
}