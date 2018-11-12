//----------------------------------------------------------------------------------
// JsonFrame
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using ServiceStack.Text;

namespace MultipartMessages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ZeroMQ;

    public static class JsonFrame
    {
        public static ZFrame Serialize<T>(T messageObject)
        {
            var message = JsonSerializer.SerializeToString<T>(messageObject);
            return new ZFrame(Encoding.UTF8.GetBytes(message));
        }

        public static T DeSerialize<T>(ZFrame frame)
        {
            var messageObject = JsonSerializer.DeserializeFromString<T>(frame.ReadString(Encoding.UTF8));
            return messageObject;
        }
    }
}
