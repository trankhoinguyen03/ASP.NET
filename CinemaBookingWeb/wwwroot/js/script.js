//let slideIndex = 1;
//showSlides(slideIndex);

//// Next/previous controls
//function plusSlides(n) {
//    showSlides(slideIndex += n);
//}

//// Thumbnail image controls
//function currentSlide(n) {
//    showSlides(slideIndex = n);
//}

//function showSlides() {
//    var slideIndex = 0; // Đảm bảo rằng slideIndex đã được khởi tạo
//    var slides = document.getElementsByClassName("mySlides");

//    if (slides.length > 0) { // Kiểm tra xem có slide nào không
//        for (let i = 0; i < slides.length; i++) {
//            slides[i].style.display = "none"; // Đảm bảo rằng slides[i] không undefined
//        }
//        slideIndex++;
//        if (slideIndex > slides.length) { slideIndex = 1; }
//        slides[slideIndex - 1].style.display = "block"; // Lỗi có thể xảy ra nếu slideIndex không hợp lệ
//    }
//}