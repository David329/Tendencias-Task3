module.exports = function (app) {

    var t1 = 0;
    var t2 = 0;
    var t3 = 0;
    //POST
    addPedido = function (req, res) {

        var JsonObject = JSON.parse(req.body.pedido);

        if (JsonObject.tipo == "omnicanal") t1++;
        else if (JsonObject.tipo == "creditcard") t2++;
        else if (JsonObject.tipo == "cash") t3++;

        // console.log(JsonObject.tipo);
        // console.log(JsonObject.prod.nombre);
        console.log("Omnicanal: " + t1.toString() + " CreditCard: " + t2.toString() + " Cash: " + t3.toString());
        res.send("null");//no es necesario enviarle una respuesta a rec1,2,3
    };
    getPedido = function (req, res) {
        //res.send("Omnicanal: " + t1.toString() + " CreditCard: " + t2.toString() + " Cash: " + t3.toString());
        res.setHeader('Content-Type', 'application/json');
        res.send(JSON.stringify([
            {tipo: "Omnicanal", cantidad: t1},
            {tipo: "CreditCard", cantidad: t2},
            {tipo: "Cash", cantidad: t3},
        ]));
    };

    //API Routes Pedidos
    app.post('/pedidos', addPedido);
    app.get('/pedidos', getPedido);
}