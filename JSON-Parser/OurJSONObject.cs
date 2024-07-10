using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSON_Parser
{
    public class OurJSONObject : IJSONObject
    {  
        public string _jsonString;
        private Dictionary<object, object> _backingDict = new();
        public OurJSONObject(string jsonString)
        {
            _jsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
            var _lexer = new Lexer(jsonString);
            var _parser = new Parser(_lexer);
            _backingDict = _parser.Parse() as Dictionary<object, object> ?? new Dictionary<object, object>();
        }

        public Dictionary<object, object> GetJSONObject()
        {
            return _backingDict;
        }
    }

    public interface IJSONObject
    {
        Dictionary<object, object> GetJSONObject();
    }
}