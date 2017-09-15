module.exports = function (app) {

    //POST
    addUsuario = function (req, res) {

var JsonObject= JSON.parse(req.body.pedido);
console.log(JsonObject.tipo);
console.log(JsonObject.prod.nombre);
// console.log(req.body.pedido);

        res.send("asd");
    };

    //API Routes Usuarios
    app.post('/usuarios', addUsuario);
}