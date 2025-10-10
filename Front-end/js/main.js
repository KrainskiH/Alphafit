const canvas = document.getElementById('bg');
const ctx = canvas.getContext('2d');
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;

const particles = [];
for(let i=0; i<50; i++){
  particles.push({
    x: Math.random() * canvas.width,
    y: Math.random() * canvas.height,
    r: Math.random()*3 + 1,
    d: Math.random()*2
  });
}

function draw() {
  ctx.clearRect(0,0,canvas.width, canvas.height);
  ctx.fillStyle = 'rgba(255,255,255,0.7)';
  ctx.beginPath();
  particles.forEach(p => {
    ctx.moveTo(p.x, p.y);
    ctx.arc(p.x, p.y, p.r, 0, Math.PI*2, true);
  });
  ctx.fill();
  update();
}

function update(){
  particles.forEach(p => {
    p.y += p.d;
    if(p.y > canvas.height) p.y = 0;
  });
}

setInterval(draw, 33);
window.addEventListener('resize', ()=>{
  canvas.width = window.innerWidth;
  canvas.height = window.innerHeight;
});

const titulo = document.getElementById("TITULO");
  let pos = 0;        // posição vertical
  let dir = 1;        // direção (1 = sobe, -1 = desce)
  const amplitude = 3; // altura do movimento (px)
  const velocidade = 0.1; // velocidade da animação

  function animar() {
    pos += dir * velocidade;
    if (pos > amplitude || pos < -amplitude) dir *= -1; // inverte o movimento
    titulo.style.transform = `translateY(${pos}px)`;
    requestAnimationFrame(animar); // loop suave
  }

  animar();

  window.addEventListener("scroll", () => {
  const header = document.querySelector("header");
  if (window.scrollY > 50) {
    header.classList.add("scrolled");
  } else {
    header.classList.remove("scrolled");
  }
});