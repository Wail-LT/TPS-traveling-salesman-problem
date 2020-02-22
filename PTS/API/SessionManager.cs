using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.API
{
    public static class SessionManager
    {
        private static Dictionary<int, Session> sessionList = new Dictionary<int, Session>();
        public static int NewSession()
        {
            Session newSession = new Session();
            int token = newSession.GetHashCode();

            sessionList.Add(token, newSession);

            return token;
        }

        public static Session GetSession(int token)
        {
            if (sessionList.ContainsKey(token))
            {
                sessionList[token].LastUse = DateTime.UtcNow;
                return sessionList[token];
            }

            throw new NotImplementedException("Session does not exist");
        }
    }
}
