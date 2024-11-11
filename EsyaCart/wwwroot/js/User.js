const changeEditable = (e) => {
    e.preventDefault();
    const elements = document.querySelectorAll('.sp1');
    elements.forEach(element => {
        element.setAttribute('contentEditable', 'true');
        element.style.border = '1px dashed #000'; // Optional: to visually indicate edit mode
    });
};

