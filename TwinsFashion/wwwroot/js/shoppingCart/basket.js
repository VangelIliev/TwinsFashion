$(document).ready(function() {
    function updateCartTotal() {
        var total = 0;
        $('.table_row').each(function() {
            var row = $(this);
            // The 6th column (index 5) contains the row total, e.g., "110.00 лв"
            var totalText = row.find('td').eq(5).text();
            var price = parseFloat(totalText.replace('лв', '').trim());
            if (!isNaN(price)) {
                total += price;
            }
        });
        $('.total').text(total.toFixed(2) + ' лв');
    }

    // Calculate total on page load
    updateCartTotal();

    $(document).on('click', '.remove-from-cart', function () {
        var button = $(this);
        var id = button.data('id');
        var size = button.data('size');
        $.ajax({
            url: '/Home/RemoveFromCart',
            type: 'POST',
            data: { id: id, size: size },
            success: function () {
                // Remove the row from the DOM for a smoother UX
                button.closest('tr').remove();
                // Recalculate the total
                updateCartTotal();
            },
            error: function() {
                alert('Възникна грешка при премахването на продукта.');
            }
        });
    });
});
