var productListAllData = [{
    id: 1,
    product: {
        img: "../assets/images/products/image.png",
        title: "Placa de acero - 4x4 in",
        category: "Xyz"
    },
    stock: "12",
    price: "215.00",
    orders: "48",
    published: {
        publishDate: "12 Oct, 2021",
        publishTime: "10:05 AM"
    }
}, {
    id: 2,
    product: {
        img: "../assets/images/products/image.png",
        title: "Tubo de aluminio - Ø2 in x 6 ft",
        category: "Xyz"
    },
    stock: "06",
    price: "160.00",
    orders: "30",
    published: {
        publishDate: "06 Jan, 2021",
        publishTime: "01:31 PM"
    }
}, {
    id: 3,
    product: {
        img: "../assets/images/products/image.png",
        title: "Barra cuadrada de acero inoxidable - 2x2 in x 4 ft",
        category: "Xyz"
    },
    stock: "10",
    price: "125.00",
    orders: "48",
    published: {
        publishDate: "26 Mar, 2021",
        publishTime: "11:40 AM"
    }
}, {
    id: 4,
    product: {
        img: "../assets/images/products/image.png",
        title: "Perfil en L de hierro - 1x1 in x 10 ft",
        category: "Xyz"
    },
    stock: "15",
    price: "340.00",
    orders: "40",
    published: {
        publishDate: "19 Apr, 2021",
        publishTime: "02:51 PM"
    }
}, {
    id: 5,
    product: {
        img: "../assets/images/products/image.png",
        title: "Lámina de cobre - 12x12 in",
        category: "Xyz"
    },
    stock: "08",
    price: "175.00",
    orders: "55",
    published: {
        publishDate: "30 Mar, 2021",
        publishTime: "09:42 AM"
    }
}, {
    id: 6,
    product: {
        img: "../assets/images/products/image.png",
        title: "Barra redonda de bronce - Ø1 in x 5 ft",
        category: "Xyz"
    },
    stock: "15",
    price: "225.00",
    orders: "48",
    published: {
        publishDate: "12 Oct, 2021",
        publishTime: "04:55 PM"
    }
}, {
    id: 7,
    product: {
        img: "../assets/images/products/image.png",
        title: "Tubo de acero al carbono - Ø4 in x 8 ft",
        category: "Xyz"
    },
    stock: "12",
    price: "105.00",
    orders: "45",
    published: {
        publishDate: "15 May, 2021",
        publishTime: "03:40 PM"
    }
}, {
    id: 8,
    product: {
        img: "../assets/images/products/image.png",
        title: "Ángulo de acero galvanizado - 3x3 in x 12 ft",
        category: "Xyz"
    },
    stock: "20",
    price: "120.00",
    orders: "48",
    published: {
        publishDate: "21 Jun, 2021",
        publishTime: "12:18 PM"
    }
}, {
    id: 9,
    product: {
        img: "../assets/images/products/image.png",
        title: "Hoja de aluminio perforada - 24x24 in",
        category: "Xyz"
    },
    stock: "14",
    price: "325.00",
    orders: "55",
    published: {
        publishDate: "15 Jan, 2021",
        publishTime: "10:29 PM"
    }
}, {
    id: 10,
    product: {
        img: "../assets/images/products/image.png",
        title: "Perfil rectangular de acero - 2x1 in x 8 ft",
        category: "Xyz"
    },
    stock: "20",
    price: "180.00",
    orders: "60",
    published: {
        publishDate: "15 Jun, 2021",
        publishTime: "03:51 PM"
    }
}, {
    id: 11,
    product: {
        img: "../assets/images/products/image.png",
        title: "Tubo cuadrado de níquel - 3x3 in x 6 ft",
        category: "Xyz"
    },
    stock: "12",
    price: "215.00",
    orders: "48",
    published: {
        publishDate: "12 Oct, 2021",
        publishTime: "10:05 AM"
    }
}, {
    id: 12,
    product: {
        img: "../assets/images/products/image.png",
        title: "Urban Ladder Pashe Chair",
        category: "Xyz"
    },
    stock: "06",
    price: "160.00",
    orders: "30",
    published: {
        publishDate: "06 Jan, 2021",
        publishTime: "01:31 PM"
    }
}],
inputValueJson = sessionStorage.getItem("inputValue");
inputValueJson && (inputValueJson = JSON.parse(inputValueJson), Array.from(inputValueJson).forEach(e => {
productListAllData.unshift(e)
}));
var editinputValueJson = sessionStorage.getItem("editInputValue");
editinputValueJson && (editinputValueJson = JSON.parse(editinputValueJson), productListAllData = productListAllData.map(function(e) {
return e.id == editinputValueJson.id ? editinputValueJson : e
})), document.getElementById("addproduct-btn").addEventListener("click", function() {
sessionStorage.setItem("editInputValue", "")
});
var productListAll = new gridjs.Grid({
    columns: [{
        name: "Product",
        width: "360px",
        data: function(e) {
            return gridjs.html('<div class="d-flex align-items-center"><div class="flex-shrink-0 me-3"><div class="avatar-sm bg-light rounded p-1"><img src="' + e.product.img + '" alt="" class="img-fluid d-block"></div></div><div class="flex-grow-1"><h5 class="fs-14 mb-1"><a href="apps-ecommerce-product-details.html" class="text-dark">' + e.product.title + '</a></h5><p class="text-muted mb-0">Category : <span class="fw-medium">' + e.product.category + "</span></p></div></div>")
        }
    }, {
        name: "Stock",
        width: "94px"
    }, {
        name: "Price",
        width: "101px",
        formatter: function(e) {
            return gridjs.html("$" + e)
        }
    }, {
        name: "Orders",
        width: "84px"
    }, {
        name: "Last restock",
        width: "220px",
        data: function(e) {
            return gridjs.html(e.published.publishDate + '<small class="text-muted ms-1">' + e.published.publishTime + "</small>")
        }
    }, {
        name: "Action",
        width: "80px",
        sort: {
            enabled: !1
        },
        formatter: function(e, t) {
            return gridjs.html('<div class="dropdown"><button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="ri-more-fill"></i></button><ul class="dropdown-menu dropdown-menu-end"><li><a class="dropdown-item" href="apps-ecommerce-product-details.html"><i class="ri-eye-fill align-bottom me-2 text-muted"></i> View</a></li><li><a class="dropdown-item edit-list" data-edit-id=' + t + ' href="apps-ecommerce-add-product.html"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li><li class="dropdown-divider"></li><li><a class="dropdown-item remove-list" href="#" data-id=' + t + ' data-bs-toggle="modal" data-bs-target="#removeItemModal"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li></ul></div>')
        }
    }],
    className: {
        th: "text-muted"
    },
    pagination: {
        limit: 10
    },
    sort: !0,
    data: productListAllData
}).render(document.getElementById("table-product-list-all")),
productListPublishedData = [{
    id: 1,
    product: {
        img: "../assets/images/products/img-2.png",
        title: "Placa de hierro fundido - 8x8 in",
        category: "Xyz"
    },
    stock: "06",
    price: "160.00",
    orders: "30",
    published: {
        publishDate: "06 Jan, 2021",
        publishTime: "01:31 PM"
    }
}, {
    id: 2,
    product: {
        img: "../assets/images/products/img-6.png",
        title: "Varilla roscada de acero galvanizado - Ø0.5 in x 4 ft",
        category: "Xyz"
    },
    stock: "15",
    price: "125.00",
    orders: "48",
    published: {
        publishDate: "12 Oct, 2021",
        publishTime: "04:55 PM"
    }
}, {
    id: 3,
    product: {
        img: "../assets/images/products/img-4.png",
        title: "Barra plana de acero inoxidable - 1x0.25 in x 6 ft",
        category: "Xyz"
    },
    stock: "15",
    price: "140.00",
    orders: "40",
    published: {
        publishDate: "19 Apr, 2021",
        publishTime: "02:51 PM"
    }
}, {
    id: 4,
    product: {
        img: "../assets/images/products/img-4.png",
        title: "Canal U de acero - 3x1.5 in x 10 ft",
        category: "Xyz"
    },
    stock: "10",
    price: "125.00",
    orders: "48",
    published: {
        publishDate: "26 Mar, 2021",
        publishTime: "11:40 AM"
    }
}, {
    id: 5,
    product: {
        img: "../assets/images/products/img-5.png",
        title: "Tubo rectangular de aluminio - 2x1 in x 8 ft",
        category: "Xyz"
    },
    stock: "08",
    price: "135.00",
    orders: "55",
    published: {
        publishDate: "30 Mar, 2021",
        publishTime: "09:42 AM"
    }
}],
productListPublished = new gridjs.Grid({
    columns: [{
        name: "Product",
        width: "360px",
        data: function(e) {
            return gridjs.html('<div class="d-flex align-items-center"><div class="flex-shrink-0 me-3"><div class="avatar-sm bg-light rounded p-1"><img src="' + e.product.img + '" alt="" class="img-fluid d-block"></div></div><div class="flex-grow-1"><h5 class="fs-14 mb-1"><a href="apps-ecommerce-product-details.html" class="text-dark">' + e.product.title + '</a></h5><p class="text-muted mb-0">Category : <span class="fw-medium">' + e.product.category + "</span></p></div></div>")
        }
    }, {
        name: "Stock",
        width: "94px"
    }, {
        name: "Price",
        width: "101px",
        formatter: function(e) {
            return gridjs.html("$" + e)
        }
    }, {
        name: "Orders",
        width: "84px"
    }, {
        name: "Last restock",
        width: "220px",
        data: function(e) {
            return gridjs.html(e.published.publishDate + '<small class="text-muted ms-1">' + e.published.publishTime + "</small>")
        }
    }, {
        name: "Action",
        width: "80px",
        sort: {
            enabled: !1
        },
        formatter: function(e, t) {
            return gridjs.html('<div class="dropdown"><button class="btn btn-soft-secondary btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="ri-more-fill"></i></button><ul class="dropdown-menu dropdown-menu-end"><li><a class="dropdown-item" href="apps-ecommerce-product-details.html"><i class="ri-eye-fill align-bottom me-2 text-muted"></i> View</a></li><li><a class="dropdown-item" href="apps-ecommerce-add-product.html"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i> Edit</a></li><li class="dropdown-divider"></li><li><a class="dropdown-item remove-list" href="#" data-id=' + t._cells[0].data + ' data-bs-toggle="modal" data-bs-target="#removeItemModal"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i> Delete</a></li></ul></div>')
        }
    }],
    className: {
        th: "text-muted"
    },
    pagination: {
        limit: 10
    },
    sort: !0,
    data: productListPublishedData
}).render(document.getElementById("table-product-list-published")),
searchProductList = document.getElementById("searchProductList");
searchProductList.addEventListener("keyup", function() {
var e = searchProductList.value.toLowerCase();

function t(e, t) {
    return e.filter(function(e) {
        return -1 !== e.product.title.toLowerCase().indexOf(t.toLowerCase())
    })
}
var i = t(productListAllData, e),
    e = t(productListPublishedData, e);
productListAll.updateConfig({
    data: i
}).forceRender(), productListPublished.updateConfig({
    data: e
}).forceRender(), checkRemoveItem()
}), Array.from(document.querySelectorAll(".filter-list a")).forEach(function(r) {
r.addEventListener("click", function() {
    var e = document.querySelector(".filter-list a.active");
    e && e.classList.remove("active"), r.classList.add("active");
    var t = r.querySelector(".listname").innerHTML,
        i = productListAllData.filter(e => e.product.category === t),
        e = productListPublishedData.filter(e => e.product.category === t);
    productListAll.updateConfig({
        data: i
    }).forceRender(), productListPublished.updateConfig({
        data: e
    }).forceRender(), checkRemoveItem()
})
});
var slider = document.getElementById("product-price-range");
noUiSlider.create(slider, {
start: [0, 2e3],
step: 10,
margin: 20,
connect: !0,
behaviour: "tap-drag",
range: {
    min: 0,
    max: 2e3
},
format: wNumb({
    decimals: 0,
    prefix: "$ "
})
});
var minCostInput = document.getElementById("minCost"),
maxCostInput = document.getElementById("maxCost"),
filterDataAll = "",
filterDataPublished = "";
slider.noUiSlider.on("update", function(e, t) {
var i = productListAllData,
    r = productListPublishedData;
t ? maxCostInput.value = e[t] : minCostInput.value = e[t];
var s = maxCostInput.value.substr(2),
    a = minCostInput.value.substr(2);
filterDataAll = i.filter(e => parseFloat(e.price) >= a && parseFloat(e.price) <= s), filterDataPublished = r.filter(e => parseFloat(e.price) >= a && parseFloat(e.price) <= s), productListAll.updateConfig({
    data: filterDataAll
}).forceRender(), productListPublished.updateConfig({
    data: filterDataPublished
}).forceRender(), checkRemoveItem()
}), minCostInput.addEventListener("change", function() {
slider.noUiSlider.set([null, this.value])
}), maxCostInput.addEventListener("change", function() {
slider.noUiSlider.set([null, this.value])
});
var filterChoicesInput = new Choices(document.getElementById("filter-choices-input"), {
addItems: !0,
delimiter: ",",
editItems: !0,
maxItemCount: 10,
removeItems: !0,
removeItemButton: !0
});
Array.from(document.querySelectorAll(".filter-accordion .accordion-item")).forEach(function(r) {
var s = r.querySelectorAll(".filter-check .form-check .form-check-input:checked").length;
r.querySelector(".filter-badge").innerHTML = s, Array.from(r.querySelectorAll(".form-check .form-check-input")).forEach(function(t) {
    var i = t.value;
    t.checked && filterChoicesInput.setValue([i]), t.addEventListener("click", function(e) {
        t.checked ? (s++, r.querySelector(".filter-badge").innerHTML = s, r.querySelector(".filter-badge").style.display = 0 < s ? "block" : "none", filterChoicesInput.setValue([i])) : filterChoicesInput.removeActiveItemsByValue(i)
    }), filterChoicesInput.passedElement.element.addEventListener("removeItem", function(e) {
        e.detail.value == i && (t.checked = !1, s--, r.querySelector(".filter-badge").innerHTML = s, r.querySelector(".filter-badge").style.display = 0 < s ? "block" : "none")
    }, !1), document.getElementById("clearall").addEventListener("click", function() {
        t.checked = !1, filterChoicesInput.removeActiveItemsByValue(i), s = 0, r.querySelector(".filter-badge").innerHTML = s, r.querySelector(".filter-badge").style.display = 0 < s ? "block" : "none", productListAll.updateConfig({
            data: productListAllData
        }).forceRender(), productListPublished.updateConfig({
            data: productListPublishedData
        }).forceRender()
    })
})
});
var searchBrandsOptions = document.getElementById("searchBrandsList");
searchBrandsOptions.addEventListener("keyup", function() {
var i = searchBrandsOptions.value.toLowerCase(),
    e = document.querySelectorAll("#flush-collapseBrands .form-check");
Array.from(e).forEach(function(e) {
    var t = e.getElementsByClassName("form-check-label")[0].innerText.toLowerCase();
    e.style.display = t.includes(i) ? "block" : "none"
})
});
var isSelected = 0;

function checkRemoveItem() {
var e = document.querySelectorAll('a[data-bs-toggle="tab"]');
Array.from(e).forEach(function(e) {
    e.addEventListener("show.bs.tab", function(e) {
        isSelected = 0, document.getElementById("selection-element").style.display = "none"
    })
}), setTimeout(function() {
    Array.from(document.querySelectorAll(".checkbox-product-list input")).forEach(function(e) {
        e.addEventListener("click", function(e) {
            1 == e.target.checked ? e.target.closest("tr").classList.add("gridjs-tr-selected") : e.target.closest("tr").classList.remove("gridjs-tr-selected");
            var t = document.querySelectorAll(".checkbox-product-list input:checked");
            isSelected = t.length, e.target.closest("tr").classList.contains("gridjs-tr-selected"), document.getElementById("select-content").innerHTML = isSelected, document.getElementById("selection-element").style.display = 0 < isSelected ? "block" : "none"
        })
    }), removeItems(), removeSingleItem()
}, 100)
}
var checkboxes = document.querySelectorAll(".checkbox-wrapper-mail input");

function removeItems() {
document.getElementById("removeItemModal").addEventListener("show.bs.modal", function(e) {
    isSelected = 0, document.getElementById("delete-product").addEventListener("click", function() {
        Array.from(document.querySelectorAll(".gridjs-table tr")).forEach(function(e) {
            var t, i = "";

            function r(e, t) {
                return e.filter(function(e) {
                    return e.id != t
                })
            }
            e.classList.contains("gridjs-tr-selected") && (t = e.querySelector(".form-check-input").value, i = r(productListAllData, t), t = r(productListPublishedData, t), productListAllData = i, productListPublishedData = t, e.remove())
        }), document.getElementById("btn-close").click(), document.getElementById("selection-element") && (document.getElementById("selection-element").style.display = "none"), checkboxes.checked = !1
    })
})
}

function removeSingleItem() {
var s;
Array.from(document.querySelectorAll(".remove-list")).forEach(function(r) {
    r.addEventListener("click", function(e) {
        s = r.getAttribute("data-id"), document.getElementById("delete-product").addEventListener("click", function() {
            function e(e, t) {
                return e.filter(function(e) {
                    return e.id != t
                })
            }
            var t = e(productListAllData, s),
                i = e(productListPublishedData, s);
            productListAllData = t, productListPublishedData = i, r.closest(".gridjs-tr").remove()
        })
    })
});
var i;
Array.from(document.querySelectorAll(".edit-list")).forEach(function(t) {
    t.addEventListener("click", function(e) {
        i = t.getAttribute("data-edit-id"), productListAllData = productListAllData.map(function(e) {
            return e.id == i && sessionStorage.setItem("editInputValue", JSON.stringify(e)), e
        })
    })
})
}