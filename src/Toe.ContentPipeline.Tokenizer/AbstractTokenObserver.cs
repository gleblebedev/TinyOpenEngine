using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public class AbstractTokenObserver<T>:ITokenObserver<T>
    {
        public virtual void OnNext(Token<T> token) { }

        public virtual void OnError(Exception exception) { }

        public virtual void OnCompleted() { }
    }
}