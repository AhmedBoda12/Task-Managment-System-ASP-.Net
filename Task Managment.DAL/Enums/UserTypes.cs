using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Task_Managment.DAL.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum UserTypes
    {
        Admin,
        User,
    }
}
