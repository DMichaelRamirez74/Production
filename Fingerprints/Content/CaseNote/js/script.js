$(document).ready(function () {



    //$('ul.accordianSubMenu').prev('a').addClass('submenu');
    $('ul.accordianMenu li').on('click', function () {
        $('ul.accordianSubMenu').removeClass('active');
        //$('ul.accordianMenu li').children('a').removeClass('current');
        $('ul.accordianSubMenu').slideUp();
        $(this).children('ul.accordianSubMenu').addClass('active');
        //$(this).children('ul.accordianSubMenu').addClass('active');

        if ($(this).children('ul.accordianSubMenu').hasClass('active')) {
            $(this).children('a').addClass('current');
            $('.active').clearQueue();
            $('.active').slideToggle();

        }


    });
   
    $(".delete-table-icon").click(function () {

        $(".delete-overlay").css({"display": "block"});
    });
    $(".bt-delete,.delete-overlay").click(function () {
        $(".delete-overlay").css({"display": "none"});
    });

});
function resise() {

    var win = $(window).height();

//    var win_width = $(window).width();

    var widleft = $(".left-side-container").width();
    var htcpy = $(".copyrights").height();
    var httphdr = $(".top-header").height();
//    var totalwid = win_width - widleft;
    $(".left-side-container").css({
        height: win
    });
    $(".right-side-container").css({
        height: win - htcpy - 40
//        width: totalwid
    });
    $(".dashboard-content-pad").css({
        height: win - httphdr - htcpy - 110

    });
  
}
function resisescroll() {

    var win = $(window).height();


    var htlogo = $(".logo-section-background").height();

    var fnht = win - htlogo;

    var htcpy = $(".copyrights").height();
    var tlht = fnht - htcpy;
    $(".dashboard-menu1").css({
        height: tlht - 42
    });

}

//$(document).ready(function () {
//
//    var win_width = $(window).width();
//    $(".nav-icon").click(function () {
//        if (win_width >= 768)
//        {
//            if ($(".left-side-container").hasClass("left-menu"))
//            {
//                $(".left-side-container").removeClass("left-menu");
//                $(".left-side-container").css({"width": 80});
//                $(".resize-menu").css({"display": "inline-block"});
//                $(".menu").css({"display": "none"});
//                $(".resize-logo").css({"display": "block"});
//                $(".menu-logo").css({"display": "none"});
//                $(".copyrights").css({"display": "none"});
//                resise();
//            } else
//            {
//                $(".left-side-container").addClass("left-menu");
//                $(".left-side-container").css({"width": 256});
//                $(".resize-menu").css({"display": "none"});
//                $(".menu").css({"display": "block"});
//                $(".resize-logo").css({"display": "none"});
//                $(".menu-logo").css({"display": "block"});
//                $(".copyrights").css({"display": "block"});
//                resise();
//            }
//        }
//
//    });

    $(".nav-icon1").click(function () {

        if ($(".left-side-container").hasClass("left-menu"))
        {
            $(".left-side-container").removeClass("left-menu");
            $(".left-side-container").css({"left": 0});
        } else
        {
            $(".left-side-container").addClass("left-menu");
            $(".left-side-container").css({"left": -282});
        }

    });

$(window).resize(function ()
{
    resise();
    resisescroll();
});
$(window).load(function ()
{
    resise();
    resisescroll();
});



