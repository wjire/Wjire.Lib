using System;

namespace Wjire.Model
{
    public abstract class BaseModel
    {
        public abstract int Id { get; }
    }

    public class Model_1 : BaseModel
    {
        public override int Id { get; }
    }
}
