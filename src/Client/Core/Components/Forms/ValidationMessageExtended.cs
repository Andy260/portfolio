using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.AspNetCore.Components.Forms
{
    /// <summary>
    /// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
    /// </summary>
    public sealed class ValidationMessageExtended<TValue> : ComponentBase, IDisposable
    {
        private EditContext? _previousEditContext;
        private Expression<Func<TValue>>? _previousFieldAccessor;
        private readonly EventHandler<ValidationStateChangedEventArgs>? _validationStateChangedHandler;
        private FieldIdentifier _fieldIdentifier;

        #region Properties

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created <c>div</c> element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Specifies the CSS class for a valid message
        /// </summary>
        [Parameter]
        public string ValidClass { get; set; } = "validation-message";

        /// <summary>
        /// Specifies the CSS class for an invalid message
        /// </summary>
        [Parameter]
        public string InvalidClass { get; set; } = "validation-message";

        /// <summary>
        /// Optional message to display upon valid validation
        /// </summary>
        [Parameter]
        public string? ValidMessage { get; set; }

        /// <summary>
        /// Specifies the field for which validation messages should be displayed.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>>? For { get; set; }

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; } = default!;

        #endregion

        #region Constructors

        /// <summary>`
        /// Constructs an instance of <see cref="ValidationMessageExtended{TValue}"/>.
        /// </summary>
        public ValidationMessageExtended()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }

        #endregion

        #region Overridden Functions

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                    $"an {nameof(EditForm)}.");
            }

            if (For == null) // Not possible except if you manually specify T
            {
                throw new InvalidOperationException($"{GetType()} requires a value for the " +
                    $"{nameof(For)} parameter.");
            }
            else if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = FieldIdentifier.Create(For);
                _previousFieldAccessor = For;
            }

            if (CurrentEditContext != _previousEditContext)
            {
                DetachValidationStateChangedListener();
                CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
                _previousEditContext = CurrentEditContext;
            }
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            IEnumerable<string> validationMessages = CurrentEditContext.GetValidationMessages(_fieldIdentifier);
            if (validationMessages.Any())
            {
                // Field is invalid
                foreach (string? message in CurrentEditContext.GetValidationMessages(_fieldIdentifier))
                {
                    BuildValidationMessage(builder, message, InvalidClass);
                }
            }
            else
            {
                // Field is valid
                if (!string.IsNullOrEmpty(ValidMessage))
                {
                    BuildValidationMessage(builder, ValidMessage, ValidClass);
                }
            }
        }

        #endregion

        #region Interface Functions

        void IDisposable.Dispose()
        {
            DetachValidationStateChangedListener();
        }

        #endregion

        #region Private Functions

        private void BuildValidationMessage(RenderTreeBuilder builder, string message, string cssClass)
        {
            builder.OpenElement(0, "div");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "class", cssClass);
            builder.AddContent(3, message);
            builder.CloseElement();
        }

        private void DetachValidationStateChangedListener()
        {
            if (_previousEditContext != null)
            {
                _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
        }

        #endregion
    }
}