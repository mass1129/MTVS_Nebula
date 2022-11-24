using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace User_Info
{
    public class CHAN_Users_Info
    {
        public static List<string> userLists = new List<string>();

        public static Dictionary<string, string> onlineUsers = new Dictionary<string, string>();
    }
}
