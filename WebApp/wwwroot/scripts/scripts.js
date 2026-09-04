(function () {
	"use strict";

	const inputSelector = "[data-customer-number-input]";

	function clearValidation(input) {
		const error = input.parentElement.querySelector("[data-customer-number-error]");

		input.setCustomValidity("");
		input.classList.remove("is-invalid");
		input.removeAttribute("aria-invalid");
		error.textContent = "";
		error.hidden = true;
	}

	function showValidation(input, result) {
		const error = input.parentElement.querySelector("[data-customer-number-error]");
		const message = result.isValid ? "" : result.error;

		input.setCustomValidity(message);
		input.classList.toggle("is-invalid", !result.isValid);
		input.setAttribute("aria-invalid", String(!result.isValid));
		error.textContent = message;
		error.hidden = result.isValid;

		return result.isValid;
	}

	async function validate(input) {
		const url = new URL(input.dataset.validationUrl, window.location.origin);
		url.searchParams.set("value", input.value);

		try {
			const response = await fetch(url, {
				headers: { "Accept": "application/json" }
			});

			if (!response.ok) {
				throw new Error("Customer number validation failed.");
			}

			return showValidation(input, await response.json());
		} catch {
			return showValidation(input, {
				isValid: false,
				error: "Customer number could not be validated."
			});
		}
	}

	function bindForm(form) {
		if (form.dataset.customerNumberValidationBound) {
			return;
		}

		form.dataset.customerNumberValidationBound = "true";
		let validationPassed = false;

		form.addEventListener("submit", async function (event) {
			if (validationPassed) {
				validationPassed = false;
				return;
			}

			event.preventDefault();
			event.stopImmediatePropagation();

			const inputs = Array.from(form.querySelectorAll(inputSelector));
			const results = await Promise.all(inputs.map(validate));

			if (!results.every(Boolean)) {
				inputs.find(input => !input.checkValidity())?.reportValidity();
				return;
			}

			validationPassed = true;
			if (event.submitter) {
				form.requestSubmit(event.submitter);
			} else {
				form.requestSubmit();
			}
		}, true);
	}

	document.addEventListener("DOMContentLoaded", function () {
		document.querySelectorAll(inputSelector).forEach(function (input) {
			input.addEventListener("input", function () {
				clearValidation(input);
			});

			input.addEventListener("blur", function () {
				void validate(input);
			});

			if (input.form) {
				bindForm(input.form);
			}
		});
	});
}());