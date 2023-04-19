//// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.
const loadPizze = filter => getPizze(filter).then(renderPizze);

const getPizze = name => axios
    .get('/api/pizza', name ? { params: { name } } : {})
    .then(res => res.data);

const renderPizze = pizze => {
    const cards = document.getElementById('pizze-filter');
    cards.innerHTML = pizze.map(pizzaComponent).join('');
}

const pizzaComponent = pizza => `
 <div class="">
    <div class="card" style="width: 18rem; height: 25rem;">
        <img src="${pizza.image}" class="card-img-top" alt="Pizza Margherita">
        <div class="card-body">
            <h5 class="card-title  ms-lg-3"><a href="/pizza/detail/${pizza.id}">${pizza.name}</a></h5>
            <p class="card-text  ms-lg-3">${pizza.description}</p>
            <h6 class="ms-lg-3">${pizza.price} $</h6>
            <h6 class="ms-lg-3">${pizza.category.title}</h6>
              <button onClick="deletePizza(${pizza.id})" type="submit" class="btn btn-danger">
                      Cancella
              </button>
        </div>
    </div>
</div>`;

const initFilter = () => {
    const filter = document.querySelector("#pizze-filter input");
    filter.addEventListener("input", (e) => loadPizze(e.target.value))
};


// <Categories>

const loadCategories = () => getCategories().then(renderCategories);

const getCategories = () => axios
    .get("/api/category")
    .then(res => res.data);

const renderCategories = categories => {
    const selectCategory = document.querySelector("#category-id");
    selectCategory.innerHTML += categories.map(categoryOptionComponent).join('');
};

const categoryOptionComponent = category => `<option value=${category.id}>${category.title}</option>`;

// </Categories>

// <Ingredients>

const loadIngredients = () => getIngredients().then(renderIngredients);

const getIngredients = () => axios
    .get("/api/ingredient")
    .then(res => res.data);

const renderIngredients = ingredients => {
    const IngredientsSelection = document.querySelector("#ingredients");
    IngredientsSelection.innerHTML = ingredients.map(ingredientOptionComponent).join('');
}

const ingredientOptionComponent = ingredient => `
	<div class="flex gap">
		<input id="${ingredient.id}" type="checkbox" />
		<label for="${ingredient.id}">${ingredient.title}</label>
	</div>`;

// </Ingredients>

// <CreatePizza>

const pizzaPizza = pizza => axios
    .pizza("/api/pizza", pizza)
    .then(() => location.href = "/pizza/apiindex")
    .catch(err => renderErrors(err.response.data.errors));

const initCreateForm = () => {
    const form = document.querySelector("#pizza-create-form");

    form.addEventListener("submit", e => {
        e.preventDefault();

        const pizza = getPizzaFromForm(form);
        pizzaPizza(pizza);
    });
};

const getPizzaFromForm = form => {
    const name = form.querySelector("#name").value;
    const description = form.querySelector("#description").value;
    const price = form.querySelector("#price").value;
    const image = form.querySelector("#image").value;
    //const imageFile = form.querySelector("#image-file");
    const categoryId = form.querySelector("#category-id").value;
    //const tags = form.querySelectorAll("#tags input");

    return {
        id: 0,
        name,
        description,
        price,
        image,
        //imageFile,
        categoryId,
        //tags
    };
};

const renderErrors = errors => {
    const titleErrors = document.querySelector("#title-errors");
    const descriptionErrors = document.querySelector("#description-errors");
    const categoryIdErrors = document.querySelector("#category-id-errors");

    titleErrors.innerText = errors.Name?.join("\n") ?? "";
    descriptionErrors.innerText = errors.Description?.join("\n") ?? "";
    categoryIdErrors.innerText = errors.CategoryId?.join("\n") ?? "";
};


const getPizza = id => axios
    .get(`/api/pizza/${id}`)
    .then(res => res.data);



function deletePizza(id) {
    axios.delete(`/Api/Pizza/${id}`)
        .then(function (response) {
            console.log(response)
        }).catch(function (error) {
            console.log(error)
        });
    location.reload()
}