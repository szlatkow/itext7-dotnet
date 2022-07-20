namespace iText.Commons.Bouncycastle.Math {
    /// <summary>
    /// This interface represents the wrapper for BigInteger that provides the ability
    /// to switch between bouncy-castle and bouncy-castle FIPS implementations.
    /// </summary>
    public interface IBigInteger {
        /// <summary>
        /// Gets integer value for the wrapped BigInteger.
        /// </summary>
        int GetIntValue();

        /// <summary>
        /// Calls toString with radix for the wrapped BigInteger.
        /// </summary>
        string ToString(int radix);
    }
}