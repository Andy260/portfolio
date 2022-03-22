using Microsoft.AspNetCore.Components.Forms;

namespace Portfolio.Client.Components.Forms
{
    /// <inheritdoc />
    public class CustomFieldClassProvider : FieldCssClassProvider
    {
        // CSS class to use for valid fields
        private readonly string _validClass;
        // CSS class to use for invalid fields
        private readonly string _invalidClass;

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="CustomFieldClassProvider"/>
        /// </summary>
        /// <param name="validClass">CSS class to use for valid fields</param>
        /// <param name="invalidClass">CSS class to use for invalid fields</param>
        public CustomFieldClassProvider(string validClass, string invalidClass)
        {
            _validClass     = validClass;
            _invalidClass   = invalidClass;
        }

        #endregion

        #region Overridden Functions

        /// <inheritdoc />
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            bool isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();
            return isValid ? _validClass : _invalidClass;
        }

        #endregion
    }
}
