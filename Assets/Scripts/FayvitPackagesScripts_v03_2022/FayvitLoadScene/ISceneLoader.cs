using System;

namespace FayvitLoadScene
{
    public interface ISceneLoader
    {
        void CenaDoCarregamento(int indice, Action acaoFinalizadora = null);
        void Update();
    }
}