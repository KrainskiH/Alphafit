

  window.addEventListener('scroll', () => {
    const header = document.querySelector('header');
    const scrollY = window.scrollY;

    if (scrollY > 10) {
      header.classList.add('scrolled');
    } else {
      header.classList.remove('scrolled');
    }
  });
const track = document.querySelector('.carousel-track');
const next = document.querySelector('.next');
const prev = document.querySelector('.prev');
const cards = document.querySelectorAll('.cardcarrossel');

let index = 0;

function updateCarousel() {
  track.style.transform = `translateX(-${index * 100}%)`;
}

next.addEventListener('click', () => {
  index = (index + 1) % cards.length; // volta ao 0 depois do último
  updateCarousel();
});

prev.addEventListener('click', () => {
  index = (index - 1 + cards.length) % cards.length; // vai pro último se estiver no 0
  updateCarousel();
});

