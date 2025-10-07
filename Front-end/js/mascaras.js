document.addEventListener('DOMContentLoaded', function () {
  const cpfInput = document.getElementById('cpf');
  const cpfErro = document.getElementById('cpf-erro');

  cpfInput.addEventListener('input', function () {
    const rawValue = this.value;
    let cleanedValue = rawValue.replace(/\D/g, '');

    /*olha se tem letras*/
    if (/[a-zA-Z]/.test(rawValue)) {
      cpfErro.style.display = 'block'; /*mostra o recado*/
    } else {
      cpfErro.style.display = 'none';  /*esconde o recado*/
    }

    if (cleanedValue.length > 11) {
      cleanedValue = cleanedValue.slice(0, 11);
    }

    let formattedCPF = cleanedValue
      .replace(/(\d{3})(\d)/, '$1.$2')
      .replace(/(\d{3})(\d)/, '$1.$2')
      .replace(/(\d{3})(\d{1,2})$/, '$1-$2');

    this.value = formattedCPF;
  });
  
  const telInput = document.getElementById('telefone');
  const telErro = document.getElementById('telefone-erro');

  telInput.addEventListener('input', function () {
    let value = this.value.replace(/\D/g, '');

    if (value.length > 11) {
      value = value.slice(0, 11);
    }

    /*(99) 99999-9999*/
    if (value.length >= 2) {
      value = value.replace(/^(\d{2})(\d)/, '($1) $2');
    }
    if (value.length >= 7) {
      value = value.replace(/(\d{5})(\d{1,4})$/, '$1-$2');
    }

    this.value = value;

    /*vai dar rro se o telefone estiver incompleto*/
    if (this.value.length < 14) {
      telErro.style.display = 'block';
      telInput.classList.add('erro-campo');
    } else {
      telErro.style.display = 'none';
      telInput.classList.remove('erro-campo');
    }
  });
});
