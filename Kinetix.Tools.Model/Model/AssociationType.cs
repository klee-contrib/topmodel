namespace TopModel.Core
{
    public enum AssociationType
    {
        /// <summary>
        /// One to many.
        /// </summary>
        OneToMany,

        /// <summary>
        /// One to one.
        /// </summary>
        OneToOne,

        /// <summary>
        /// Many to one.
        /// </summary>
        ManyToOne,

        /// <summary>
        /// Many to many.
        /// </summary>
        ManyToMany
    }
}