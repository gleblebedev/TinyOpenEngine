using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public interface ITokenObserver<T>
    {
        void OnNext(Token<T> token);

        void OnError(Exception exception);

        void OnCompleted();
    }
}
