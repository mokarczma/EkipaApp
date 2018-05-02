$('.images-slider').slick({
	slidesToShow: 3,
	slidesToScroll: 2,
	arrows: false,
	dots: true,
	infinite: true,
	variableWidth: true,
});
$('.single-item').slick();

	$(function() {
		$('.editModal').modal();
	});

		function editProduct(id) {
		$.ajax({
			url: '/Account/PasswordEdit/' + productId, // The method name + paramater
			success: function (data) {
				$('#modalWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
			}
		});
	}
