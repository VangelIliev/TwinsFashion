
    $(document).on('click', '.remove-from-cart', function () {
        var button = $(this);
        var id = button.data('id');
        var size = button.data('size');
        $.ajax({
            url: '/Home/RemoveFromCart',
            type: 'POST',
            data: { id: id, size: size },
            success: function () {
                // Option 1: Reload the page to update the cart
                location.reload();
                // Option 2: Remove the row from the DOM (for a smoother UX)
                // button.closest('tr').remove();
            }
        });
    });
