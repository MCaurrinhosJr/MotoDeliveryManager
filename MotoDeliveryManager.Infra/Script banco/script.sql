CREATE DATABASE MotoDeliveryManager;

CREATE TABLE "Moto" (
    "Id" SERIAL PRIMARY KEY,
    "Placa" VARCHAR(255) NOT NULL,
    "Marca" VARCHAR(255) NOT NULL,
    "Modelo" VARCHAR(255) NOT NULL,
    "Ano" VARCHAR(4) NOT NULL
);

CREATE TABLE "Pedido" (
    "Id" SERIAL PRIMARY KEY,
    "DataCriacao" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ValorCorrida" NUMERIC(18,2) NOT NULL,
    "Endereco" VARCHAR(255) NOT NULL,
    "StatusPedido" VARCHAR(255),
    "EntregadorId" INT
);

CREATE TABLE "Entregador" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(255) NOT NULL,
    "CNPJ" VARCHAR(14) NOT NULL,
    "NumeroCNH" VARCHAR(20) NOT NULL,
    "DataNascimento" DATE NOT NULL,
    "TipoCNH" VARCHAR(255) NOT NULL,
    "FotoCNHUrl" VARCHAR(255)
);

CREATE TABLE "Locacao" (
    "Id" SERIAL PRIMARY KEY,
    "DataInicio" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "DataTerminoPrevista" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "DataTerminoReal" TIMESTAMP WITHOUT TIME ZONE,
    "ValorTotalPrecisto" NUMERIC(18,2) NOT NULL,
    "ValorTotal" NUMERIC(18,2),
    "EntregadorId" INT NOT NULL,
    "MotoId" INT NOT NULL,
    "Status" VARCHAR(255) NOT NULL
);

CREATE TABLE "Notificacao" (
    "Id" SERIAL PRIMARY KEY,
    "Mensagem" TEXT NOT NULL,
    "DataEnvio" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "EntregadorId" INT NOT NULL,
    "PedidoId" INT NOT NULL
);

ALTER TABLE "Pedido" ADD CONSTRAINT "FK_Pedido_Entregador_EntregadorId" FOREIGN KEY ("EntregadorId") REFERENCES "Entregador" ("Id");
ALTER TABLE "Locacao" ADD CONSTRAINT "FK_Locacao_Entregador_EntregadorId" FOREIGN KEY ("EntregadorId") REFERENCES "Entregador" ("Id");
ALTER TABLE "Locacao" ADD CONSTRAINT "FK_Locacao_Moto_MotoId" FOREIGN KEY ("MotoId") REFERENCES "Moto" ("Id");
ALTER TABLE "Notificacao" ADD CONSTRAINT "FK_Notificacao_Entregador_EntregadorId" FOREIGN KEY ("EntregadorId") REFERENCES "Entregador" ("Id");
ALTER TABLE "Notificacao" ADD CONSTRAINT "FK_Notificacao_Pedido_PedidoId" FOREIGN KEY ("PedidoId") REFERENCES "Pedido" ("Id");

CREATE INDEX "IX_Moto_Placa" ON "Moto" ("Placa");