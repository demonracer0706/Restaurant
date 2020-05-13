function main() {

    (function () {
        'use strict';

        $('a.page-scroll').click(function () {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top - 40
                    }, 900);
                    return false;
                }
            }
        });


        // Show Menu on Book
        $(window).bind('scroll', function () {
            var navHeight = $(window).height() - 500;
            if ($(window).scrollTop() > navHeight) {
                $('.navbar-default').addClass('on');
            } else {
                $('.navbar-default').removeClass('on');
            }
        });

        $('body').scrollspy({
            target: '.navbar-default',
            offset: 80
        });

        // Hide nav on click
        $(".navbar-nav li a").click(function (event) {
            // check if window is small enough so dropdown is created
            var toggle = $(".navbar-toggle").is(":visible");
            if (toggle) {
                $(".navbar-collapse").collapse('hide');
            }
        });

        // Portfolio isotope filter
        $(window).load(function () {
            var $container = $('.portfolio-items');
            $container.isotope({
                filter: '*',
                animationOptions: {
                    duration: 750,
                    easing: 'linear',
                    queue: false
                }
            });
            $('.cat a').click(function () {
                $('.cat .active').removeClass('active');
                $(this).addClass('active');
                var selector = $(this).attr('data-filter');
                $container.isotope({
                    filter: selector,
                    animationOptions: {
                        duration: 750,
                        easing: 'linear',
                        queue: false
                    }
                });
                return false;
            });

        });

        // datettimepicker
        $('#datetimepicker').datetimepicker({
            format: "DD/MM/YYYY HH:mm",
            locale: "vi"
        });
    }());
}
main();
$(document).ready(() => {
    function validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }
    // bookATable
    var form = document.getElementById("frmOrder");
    // Loop over them and prevent submission
    $(form).on("submit", function (event) {
        $(".text-danger").text("");
        event.preventDefault();
        const name = $("#name").val();
        const email = $("#email").val();
        const tableId = $("#select-menu-item").val();
        const date = $("#input-date").val();
        //if (!validateEmail(email)) {
        //    $("#emailInvalid").text("Sai định dạng email!");
        //    form.set
        //}
        if (form.checkValidity() === false) {
            event.stopPropagation();
            if (name === "") {
                $("#nameInvalid").text("Enter Full Name!");
            }
            if (date === "") {
                $("#dateInvalid").text("Enter Time!");
            }
            if (email === "") {
                $("#emailInvalid").text("Emter Email!");
            }
        }
        else {
            $.ajax(
                {
                    url: "/Home/BookATable",
                    method: "post",
                    data: {
                        name,
                        phone,
                        email,
                        tableId,
                        date
                        },
                    complete: () => {
                        $("#modalContent").text("Đặt bàn thành công! Cabin Coffee xin cảm ơn quý khách!");
                        $("#orderModal").modal({
                            show: "true"
                        });
                        return;
                    }
                }
            );
        }
    });
})
