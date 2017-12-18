$(document).ready(function() {

    $(".actionlink").on("click", function (e) {
        var targetLink = $(e.target)[0].dataset.actionurl;
        $(".loading").toggle();
        $(".content-area").load("/home/" + targetLink, function() {
            $(".loading").toggle();
        });  
    });

    
});

