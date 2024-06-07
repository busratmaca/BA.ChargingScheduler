namespace Shared.Kernel.Abstracts
{

    public interface IBaseCommand<in TRequestModel, TCommand>
                    where TRequestModel : IBaseRequestModel
                    where TCommand : IBaseCommand<TRequestModel, TCommand>
    {
        static abstract TCommand FromRequest(TRequestModel request);
    }

}
