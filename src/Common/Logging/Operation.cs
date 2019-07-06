// <copyright file="Operation.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    /// Use this as a scope to log
    /// </summary>
    public class Operation : IDisposable
    {
        private readonly OperationData operationData;
        private readonly DateTime startTime;
        private readonly IDictionary<string, object> propertyBag;
        private readonly IOperationLogger operationLogger;
        private readonly string dataCenter;

        private ErrorLevel errorLevel;
        private string message = string.Empty;
        private string errorSignature = string.Empty;
        private OperationResult operationResult;
        private string exception;
        private string tenantId = string.Empty;

        public string StatusCode { get; set; } = string.Empty;

        // For test use: We don't want to mark as failure operations that have been triggered by negative runner scenarios
        // So by setting this bool to true we ensure that this operation will be logged anyway as success
        public bool ExpectedToFail { get; set; } = false;

        internal Operation(
            OperationData operationData,
            string dataCenterName,
            IOperationLogger operationLogger)
        {
            this.operationData = operationData;
            this.startTime = DateTime.UtcNow;
            this.propertyBag = new Dictionary<string, object>();
            this.operationLogger = operationLogger;
            this.errorLevel = ErrorLevel.None;
            this.dataCenter = dataCenterName;

            // Until explicitly marked as Failure, the operation result is always a Success
            this.operationResult = OperationResult.Success;

            // Only failure will have exception
            this.exception = string.Empty;
        }

        public void LogFailure(
            ErrorLevel errorLevel,
            Exception exception,
            string errorMessage = "")
        {
            this.errorSignature = exception.GetType().Name;
            this.message = $"Exception: {exception.Message}. {errorMessage}";
            this.errorLevel = errorLevel;
            this.exception = exception.ToString();

            this.operationResult = OperationResult.Failure;
        }

        /// <summary>
        /// Add list of properties to property bag
        /// </summary>
        /// <param name="properties">List of key and value pairs</param>
        public void AddProperties(IDictionary<string, object> properties)
        {
            foreach (KeyValuePair<string, object> prop in properties)
            {
                this.AddProperty(prop.Key, prop.Value);
            }
        }

        /// <summary>
        /// Add list of properties to property bag
        /// </summary>
        /// <param name="properties">List of key and value pairs</param>
        public void AddProperties(IDictionary<string, string> properties)
        {
            foreach (KeyValuePair<string, string> prop in properties)
            {
                this.AddProperty(prop.Key, prop.Value);
            }
        }

        /// <summary>
        /// Add list of properties to property bag
        /// </summary>
        /// <param name="properties">List of key and value pairs</param>
        public void AddProperties(IDictionary<string, DateTime> properties)
        {
            foreach (KeyValuePair<string, DateTime> prop in properties)
            {
                this.AddProperty(prop.Key, prop.Value);
            }
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, object value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, value, overwrite);
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, int value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, value.ToString(CultureInfo.InvariantCulture), overwrite);
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, long value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, value.ToString(CultureInfo.InvariantCulture), overwrite);
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, bool value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, value.ToString(CultureInfo.InvariantCulture), overwrite);
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, string value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, value, overwrite);
        }

        /// <summary>
        /// Add property to property bag
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="overwrite">Indicates if the property value should be overwritten or appended to</param>
        public void AddProperty(string key, DateTime value, bool overwrite = false)
        {
            this.AddPropertyInternal(key, GetLogFormattedTimestamp(value), overwrite);
        }

        /// <summary>
        /// Runs the operation and logs when there is an exception
        /// </summary>
        /// <param name="action">Method logic</param>
        public void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                this.LogFailure(
                    ErrorLevel.Error,
                    exception: ex);

                throw;
            }
        }

        /// <summary>
        /// Runs the operation and logs when there is an exception
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">Method logic</param>
        /// <returns>Whatever the method logic returns</returns>
        public async Task<T> RunAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                this.LogFailure(
                    ErrorLevel.Error,
                    exception: ex);

                throw;
            }
        }

        /// <summary>
        /// Runs the operation and logs when there is an exception
        /// </summary>
        /// <param name="action">Method logic</param>
        public async Task RunAsync(Func<Task> action)
        {
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.LogFailure(
                    ErrorLevel.Error,
                    exception: ex);

                throw;
            }
        }

        /// <summary>
        /// Runs the operation and logs when there is an exception
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">Method logic</param>
        public T Run<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                this.LogFailure(
                    ErrorLevel.Error,
                    exception: ex);

                throw;
            }
        }

        public void SetTenantId(string id)
        {
            this.tenantId = id;
        }

        /// <summary>
        /// Internal method to add properties to Property bag
        /// </summary>
        private void AddPropertyInternal(string key, object value, bool overwrite)
        {
            if (this.propertyBag.ContainsKey(key))
            {
                List<object> list = this.propertyBag[key] is IList ? this.propertyBag[key] as List<object> : new List<object> { this.propertyBag[key] };
                list.Add(value);
                this.propertyBag[key] = overwrite ? value : list;
            }
            else
            {
                this.propertyBag.Add(key, value);
            }
        }

        /// <summary>
        /// Get the UTC formatted date upto a millisecond precision
        /// </summary>
        /// <returns>string UTC date</returns>
        public static string GetLogFormattedTimestamp(DateTime dateTime)
        {
            return dateTime.ToString("o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Whether already disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log end method during dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <remarks>https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose</remarks>
        /// <param name="disposing">whether is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // The Dispose() call is from user code
                int durationMs = (int)(DateTime.UtcNow - this.startTime).TotalMilliseconds;

                var result = this.ExpectedToFail ? OperationResult.Success : this.operationResult;

                this.operationLogger.LogOperation(
                    this.operationData,
                    this.dataCenter,
                    result,
                    this.errorLevel,
                    this.startTime,
                    this.propertyBag,
                    message: this.message,
                    errorSignature: this.errorSignature,
                    durationMs: durationMs,
                    exception: this.exception,
                    httpStatusCode: this.StatusCode,
                    tenantId: this.tenantId);
            }

            this.disposed = true;
        }
    }
}