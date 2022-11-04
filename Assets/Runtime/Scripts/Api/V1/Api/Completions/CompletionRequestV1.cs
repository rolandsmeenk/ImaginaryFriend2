﻿using OpenAi.Json;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAi.Api.V1
{
    /// <summary>
    /// Object used when requesting a completion. <see href="https://beta.openai.com/docs/api-reference/create-completion"/>
    /// </summary>
    public class CompletionRequestV1 : AModelV1
    {
        /// <summary>
        /// ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.
        /// </summary>
        public string model;
            
        /// <summary>
        /// The prompt(s) to generate completions for, encoded as a string, a list of strings, or a list of token lists. Note that<|endoftext|> is the document separator that the model sees during training, so if a prompt is not specified the model will generate as if from the beginning of a new document.
        /// </summary>
        public StringOrArray prompt;

        /// <summary>
        /// The maximum number of tokens to generate. Requests can use up to 2048 tokens shared between prompt and completion. (One token is roughly 4 characters for normal English text)
        /// </summary>
        public int? max_tokens;

        /// <summary>
        /// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. We generally recommend altering this or top_p but not both.
        /// </summary>
        public float? temperature;

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. We generally recommend altering this or temperature but not both.
        /// </summary>
        public float? top_p;

        /// <summary>
        /// How many completions to generate for each prompt. Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and ensure that you have reasonable settings for max_tokens and stop.
        /// </summary>
        public int? n;

        /// <summary>
        /// Whether to stream back partial progress. If set, tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a data: [DONE] message. <see href="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#Event_stream_format"/>
        /// </summary>
        public bool? stream;

        /// <summary>
        /// Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens. For example, if logprobs is 10, the API will return a list of the 10 most likely tokens. the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.
        /// </summary>
        public int? logprobs;

        /// <summary>
        /// Echo back the prompt in addition to the completion
        /// </summary>
        public bool? echo;

        /// <summary>
        /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
        /// </summary>
        public StringOrArray stop;

        /// <summary>
        /// Number between 0 and 1 that penalizes new tokens based on whether they appear in the text so far. Increases the model's likelihood to talk about new topics. <see href="https://beta.openai.com/docs/api-reference/parameter-details"/>
        /// </summary>
        public float? presence_penalty;

        /// <summary>
        /// Number between 0 and 1 that penalizes new tokens based on their existing frequency in the text so far. Decreases the model's likelihood to repeat the same line verbatim. <see href="https://beta.openai.com/docs/api-reference/parameter-details"/>
        /// </summary>
        public float? frequency_penalty;

        /// <summary>
        /// Generates best_of completions server-side and returns the "best" (the one with the lowest log probability per token). Results cannot be streamed. When used with n, best_of controls the number of candidate completions and n specifies how many to return – best_of must be greater than n. Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and enure that you have reasonable settings for max_tokens and stop.
        /// </summary>
        public int? best_of;

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion. Accepts a json object that maps tokens(specified by their token ID in the GPT tokenizer) to an associated bias value from -100 to 100. You can use this tokenizer tool (which works for both GPT-2 and GPT-3) to convert text to token IDs. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token. As an example, you can pass <c>{"50256": -100}</c> to prevent the <|endoftext|> token from being generated.
        /// </summary>
        public Dictionary<string, int> logit_bias;

        /// <inheritdoc />
        public override void FromJson(JsonObject json)
        {
            if (json.Type != EJsonType.Object) throw new OpenAiApiException("Deserialization failed, provided json is not an object");

            foreach(JsonObject obj in json.NestedValues)
            {
                switch (obj.Name) 
                {
                    case nameof(model):
                        model = obj.StringValue;
                        break;
                    case nameof(prompt):
                        prompt = new StringOrArray();
                        prompt.FromJson(obj);
                        break;
                    case nameof(max_tokens):
                        max_tokens = int.Parse(obj.StringValue);
                        break;
                    case nameof(temperature):
                        temperature = float.Parse(obj.StringValue);
                        break;
                    case nameof(top_p):
                        top_p = float.Parse(obj.StringValue);
                        break;
                    case nameof(n):
                        n = int.Parse(obj.StringValue);
                        break;
                    case nameof(stream):
                        stream = bool.Parse(obj.StringValue);
                        break;
                    case nameof(logprobs):
                        logprobs = int.Parse(obj.StringValue);
                        break;
                    case nameof(echo):
                        echo = bool.Parse(obj.StringValue);
                        break;
                    case nameof(stop):
                        stop = new StringOrArray();
                        stop.FromJson(obj);
                        break;
                    case nameof(presence_penalty):
                        presence_penalty = float.Parse(obj.StringValue);
                        break;
                    case nameof(frequency_penalty):
                        frequency_penalty = float.Parse(obj.StringValue);
                        break;
                    case nameof(best_of):
                        best_of = int.Parse(obj.StringValue);
                        break;
                    case nameof(logit_bias):
                        logit_bias = new Dictionary<string, int>();

                        foreach(JsonObject child in obj.NestedValues)
                        {
                            logit_bias.Add(child.Name, int.Parse(child.StringValue));
                        }
                        break;
                }
            }
        }

        /// <inheritdoc />
        public override string ToJson()
        {
            JsonBuilder jb = new JsonBuilder();

            jb.StartObject();
            jb.Add(nameof(model), model);
            jb.Add(nameof(prompt), prompt);
            jb.Add(nameof(max_tokens), max_tokens);
            jb.Add(nameof(temperature), temperature);
            jb.Add(nameof(top_p), top_p);
            jb.Add(nameof(n), n);
            jb.Add(nameof(stream), stream);
            jb.Add(nameof(logprobs), logprobs);
            jb.Add(nameof(echo), echo);
            jb.Add(nameof(stop), stop);
            jb.Add(nameof(presence_penalty), presence_penalty);
            jb.Add(nameof(frequency_penalty), frequency_penalty);
            jb.Add(nameof(best_of), best_of);
            jb.Add(nameof(logit_bias), logit_bias);
            jb.EndObject();

            return jb.ToString();
        }
    }
}
