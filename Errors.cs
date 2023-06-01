using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSharpFunctionalExtensions.Error
{
    public sealed class Error : ValueObject, ICombine
    {
        public static Error Empty => new Error("", "");

        private List<ErrorBase> _errors;

        private readonly ErrorBase _errorBase;

        public Error(string code, string message) : this(new List<ErrorBase> { new ErrorBase(code, message) }) { _errorBase = new ErrorBase(code, message); }

        private Error(List<ErrorBase> errors)
        {
            _errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public string Code => _errorBase?.Code ?? string.Join("\n\r", _errors.Select(e => e.Code));

        public string Message => _errorBase?.Message ?? string.Join("\n\r", _errors.Select(e => e.Message));

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Code;
        }

        public static implicit operator string(Error error)
        {
            return error.Message;
        }

        public IReadOnlyList<ErrorBase> Errors => _errors;

        public ICombine Combine(ICombine value)
        {
            var errorMsg = value as Error;
            var _errorList = new List<ErrorBase>(errorMsg._errors);
            _errorList.AddRange(_errors);
            return new Error(_errorList);
        }
    }
    
    public sealed class ErrorBase
    {
        public ErrorBase(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }

        public string Message { get; }
    }
}
