namespace ProducerMiddleware
{
    public readonly record struct ModelEvent(string nome, string pedido, string topico);
}