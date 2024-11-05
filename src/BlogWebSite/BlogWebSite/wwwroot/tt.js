function Lo()
{
    const row = document.getElementById('teststst');
    if (!row)
    {
        console.error('Element with ID "teststst" not found');
        return;
    }

    const bar = document.querySelector('header');
    const foot = document.querySelector('footer');

    if (!bar || !foot)
    {
        console.error('Header or footer not found');
        return;
    }

    const barH = bar.offsetHeight;
    const footH = foot.offsetHeight;

    const allH = barH + footH + 10;  // Adding a little margin (10px)

    // Set maxHeight with the correct value
    row.style.maxHeight = `calc(100vh - ${allH}px)`;

}