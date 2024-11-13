document.addEventListener('DOMContentLoaded', function () {
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const carousel = document.querySelector('.product-carousel');
    const productItems = document.querySelectorAll('.product-item'); // Select all product items
    const AddcartBtns = document.querySelectorAll('.Addcart-btn');  // Select all Add to Cart buttons
    const BuyNowBtns = document.querySelectorAll('.buy-now-btn');  // Select all Buy Now buttons
    
    // Carousel navigation
    prevBtn.addEventListener('click', () => {
        carousel.scrollBy({ left: 320, behavior: 'smooth' });
    });
    nextBtn.addEventListener('click', () => {
        carousel.scrollBy({ left: -315, behavior: 'smooth' });
    });

    
    productItems.forEach((divhoveraction, index) => {
        divhoveraction.addEventListener('mouseover', () => {
            divhoveraction.style.transform = 'translateX(0px) translateY(0px) translateZ(10px) scaleX(1) scaleY(1) scaleZ(1)'; 
            divhoveraction.style.boxShadow = '0px 0px 8px #aaa6a67d';
            AddcartBtns[index].style.display = 'block'; 
            BuyNowBtns[index].style.display = 'block';
              
        });

        divhoveraction.addEventListener('mouseout', () => {
            divhoveraction.style.transform = ''; 
            divhoveraction.style.boxShadow = '';
            AddcartBtns[index].style.display = 'none'; 
            BuyNowBtns[index].style.display = 'none'; 
        });
    });
});
