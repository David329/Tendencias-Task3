module.exports = function (app) {

    //POST
    addUsuario = function (req, res) {
        res.send(req.body.pedido);//manejar el pedido, este es un string
    };

    //API Routes Usuarios
    app.post('/usuarios', addUsuario);
}