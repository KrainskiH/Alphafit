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
