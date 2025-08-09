$(document).ready(function() {
    const $modal = $('.js-modal1');
    const $slickContainer = $modal.find('.wrap-slick3');
    const $slickElement = $slickContainer.find('.slick3');

    function initializeSlick(images) {
        if ($slickElement.hasClass('slick-initialized')) {
            $slickElement.slick('unslick');
        }
        $slickElement.html('');

        if (images && images.length > 0) {
            images.forEach(function(imageUrl) {
                const item = `
                    <div class="item-slick3" data-thumb="${imageUrl}">
                        <div class="wrap-pic-w pos-relative">
                            <img src="${imageUrl}" alt="IMG-PRODUCT">
                            <a class="flex-c-m size-108 how-pos1 bor0 fs-16 cl10 bg0 hov-btn3 trans-04" href="${imageUrl}">
                                <i class="fa fa-expand"></i>
                            </a>
                        </div>
                    </div>`;
                $slickElement.append(item);
            });
        }

        $slickElement.slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            fade: true,
            infinite: true,
            autoplay: false,
            arrows: true,
            appendArrows: $slickContainer.find('.wrap-slick3-arrows'),
            prevArrow: '<button class="arrow-slick3 prev-slick3"><i class="fa fa-angle-left" aria-hidden="true"></i></button>',
            nextArrow: '<button class="arrow-slick3 next-slick3"><i class="fa fa-angle-right" aria-hidden="true"></i></button>',
            dots: true,
            appendDots: $slickContainer.find('.wrap-slick3-dots'),
            dotsClass: 'slick3-dots',
            customPaging: function(slick, index) {
                var thumb = $(slick.$slides[index]).data('thumb');
                return '<li><img src="' + thumb + '"></li>';
            },
        });
    }

    $('.quickViewBtn').on('click', function() {
        var productId = $(this).data('id');
        $modal.attr('data-product-id', productId); // Store product ID on the modal

        $.get('/Home/ProductDetails/' + productId, function(data) {
            $('.js-name-detail').text(data.name);
            $modal.find('.mtext-106.cl2').text(data.price.toFixed(2) + ' лв');
            $modal.find('.stext-102.cl3.p-t-23').text(data.description);

            var $sizesSelect = $modal.find('.js-select2');
            $sizesSelect.empty().append('<option value="">Изберете размер</option>');
            if (data.sizes && data.sizes.length > 0) {
                data.sizes.forEach(function(size) {
                    $sizesSelect.append($('<option>', { value: size, text: size }));
                });
            }
            
            initializeSlick(data.imageUrls);
            $modal.addClass('show-modal1');
        });
    });

    $('.js-addcart-detail').on('click', function() {
        var productId = $modal.data('product-id');
        var selectedSize = $modal.find('.js-select2').val();
        var quantity = $modal.find('.num-product').val();

        if (!selectedSize) {
            alert('Моля, изберете размер.');
            return;
        }

        $.post('/Home/AddToCart', {
            Id: productId,
            Size: selectedSize,
            Quantity: quantity
        })
        .done(function(response) {
            if (response.success) {
                window.location.href = '/Home/ShoppingCart';
            } else {
                alert('Възникна грешка при добавянето на продукта.');
            }
        })
        .fail(function() {
            alert('Възникна грешка при добавянето на продукта.');
        });
    });

    $('.js-hide-modal1').on('click', function() {
        $modal.removeClass('show-modal1');
    });
});
