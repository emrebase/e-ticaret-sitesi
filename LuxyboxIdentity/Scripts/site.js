$(document).ready(function () {
    totalPriceRefresh();
});

$(".btn-decrease").on("click", function () {
    ProductQuantityChange($(this), "-");
    totalPriceRefresh();
});

$(".btn-increase").on("click", function () {
    ProductQuantityChange($(this), "+");
    totalPriceRefresh();
});

$(".btn-delete").on("click", function () {
    var $productItem = $(this).closest(".product-item");
    var productId = $productItem.attr("data-product-id");
    updateDelete(productId, $productItem);
});


function ProductQuantityChange($this, type) {
    var $productItem = $this.closest(".product-item");
    var $inputQuantity = $productItem.find(".product-quantity");
    var productId = $productItem.attr("data-product-id");
    var productPrice = $productItem.attr("data-product-price");
    
    var value = parseInt($inputQuantity.val());

    if (type === "+") {
        value = value + 1;
        if (value > 10)
            value = 10;
    } else {
        value = value - 1;
        if (value < 1)
            value = 1;
    }


    $inputQuantity.val(value);
    updateQuantity(productId, value);

    productPrice = productPrice.replace(",", ".");
    productPrice = (parseFloat(productPrice) * value).toFixed(2);
    $productItem.find(".product-price").text(productPrice);
}


function totalPriceRefresh() {
    var totalPriceElements = $(".product-price");

    var totalPrice = 0;
    totalPriceElements.each(function (index) {
        totalPrice += parseFloat($(this).text().replace(",", "."));
    });


    $(".cart-total-price").text(totalPrice.toFixed(2));
}


function updateQuantity(productId, quantity) {
    $.ajax({
        method: "POST",
        url: "/Home/CartItemQuantityUpdate",
        data: { productId: productId, quantity: quantity }
    })
        .done(function (msg) {
        });
}

function updateDelete(productId, $productItem) {
    $.ajax({
        url: "/Home/CartItemDeleteUpdate",
        type: 'POST',
        data: { productId: productId },
        success: function (r) {
            if (r.result === true) {
                $productItem.remove();
                totalPriceRefresh();
                refreshCartIconCount();
            }
            // Do something with the result
        }
    });
}
     
 

function refreshCartIconCount() {
    var $productItems = $(".product-list .product-item");   
    $(".cart-icon .cart-icon-count").text($productItems.length);
}

//function requiredcontact() {
//    var required = document.querySelectorAll("input-required");
//    required.forEach(function (element) {
//        if (element.value.trim() == "") {
//            element.style.backgroundColor = "red";
//        } else {
//            element.style.backgroundColor = "blue";
//        }
//    });
//}



$("#btnsend").on("click", function () {
    var name = $("#name").val();
    var email = $("#email").val();
    var subject = $("#subject").val();
    var message = $("#message").val();

    if (name == "") {
        $("#name").addClass("not-empty");
        $(".error-message").text("Lütfen isim giriniz.");
        return;
    } 

    if (email == "") {
        $("#email").addClass("not-empty");
        $(".error-message").text("Lütfen email giriniz.");
        return;
    } 

    if (subject == "") {
        $("#subject").addClass("not-empty");
        $(".error-message").text("Lütfen konu giriniz.");
        return;
    } 

    if (message == "") {
        $("#message").addClass("not-empty");
        $(".error-message").text("Lütfen mesaj giriniz.");
        return;
    } 



    $.ajax({
        method: "POST",
        url: "/Home/Contact",
        data: {
            name: name,
            mail: email,
            subject: subject,
            message: message
        }
    })
    .done(function (r) {
        if (r.result) {
            $(".control input, .control textarea").val("");
            $(".control input, .control textarea").removeClass("not-empty");
            $(".error-message").text("");
        }
    });
 })




Number.prototype.round = function (p) {
    p = p || 10;
    return parseFloat(this.toFixed(p));
};
