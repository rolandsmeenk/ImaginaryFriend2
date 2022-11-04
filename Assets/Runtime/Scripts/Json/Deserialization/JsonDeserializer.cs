using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpenAi.Json
{
    public static class JsonDeserializer
    {
        public static JsonObject FromJson(string json)
        {
            string[] tokens = JsonLexer.Lex(json.Replace(":", ": ")); // resolve :{timestamp} issue without delving into the lexer code
            return JsonSyntaxAnalyzer.Parse(tokens);
        }
    }
}