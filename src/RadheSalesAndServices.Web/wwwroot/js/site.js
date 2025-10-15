document.addEventListener('DOMContentLoaded', () => {
    const saleForm = document.querySelector('[data-sale-form]');
    if (!saleForm) {
        return;
    }

    const itemsContainer = saleForm.querySelector('[data-sale-items]');
    const addLineButton = saleForm.querySelector('[data-add-line]');

    const updateIndices = () => {
        const rows = itemsContainer.querySelectorAll('[data-sale-row]');
        rows.forEach((row, index) => {
            row.querySelectorAll('select, input').forEach(input => {
                const name = input.getAttribute('data-name');
                if (name) {
                    input.name = name.replace('__index__', index);
                    input.id = input.name.replace(/[\.\[\]]+/g, '_');
                }
            });
        });
    };

    itemsContainer.addEventListener('click', event => {
        const target = event.target;
        if (target.matches('[data-remove-line]')) {
            const row = target.closest('[data-sale-row]');
            row.remove();
            updateIndices();
        }
    });

    if (addLineButton) {
        addLineButton.addEventListener('click', event => {
            event.preventDefault();
            const template = saleForm.querySelector('#sale-item-template');
            if (!template) {
                return;
            }

            const clone = template.content.firstElementChild.cloneNode(true);
            itemsContainer.appendChild(clone);
            updateIndices();
        });
    }

    updateIndices();
});
