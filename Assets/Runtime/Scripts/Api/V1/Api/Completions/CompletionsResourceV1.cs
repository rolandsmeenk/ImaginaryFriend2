using System;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAi.Api.V1
{
    public class CompletionsResourceV1 : AApiResource<OpenAiApiV1>
    {
        /// <inheritdoc/>
        public override string Endpoint => "/completions";
        
        /// <summary>
        /// Construct with parent
        /// </summary>
        /// <param name="parent"></param>
        public CompletionsResourceV1(OpenAiApiV1 parent) : base(parent) { }
        
        /// <summary>
        /// Creates a completion for the provided prompt and parameters. <see cref="https://beta.openai.com/docs/api-reference/completions"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Asynchronously returns classification result</returns>
        public async Task<ApiResult<CompletionV1>> CreateCompletionAsync(CompletionRequestV1 request)
        {
            return await PostAsync<CompletionRequestV1, CompletionV1>(request);
        }
        
        /// <summary>
        /// Creates a completion for the provided prompt and parameters. <see cref="https://beta.openai.com/docs/api-reference/completions"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Asynchronously returns classification result</returns>
        public Coroutine CreateCompletionCoroutine(MonoBehaviour mono, CompletionRequestV1 request, Action<ApiResult<CompletionV1>> onResult)
        {
            return PostCoroutine(mono, request, onResult);
        }
    }
}