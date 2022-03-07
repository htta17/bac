using DatabaseContext;
using System;
using System.Collections.Generic;

namespace CoreLogic.StandardlizedAlgorithms
{
    /// <summary>
    /// Hệ số cập nhật cho đầu vào, tùy từng giải thuật để có các tên biến khác nhau
    /// </summary>
    public interface IAutoInputCoeff
    {         
    }

    /// <summary>
    /// Hệ số cập nhật cho đầu ra, tùy từng giải thuật để có các tên biến khác nhau
    /// </summary>
    public interface IAutoOutputCoeff
    { 
    
    }

    /// <summary>
    /// Interface chuẩn, tất cả các giải thuật khác đều tuân thủ
    /// </summary>
    public interface IAutoBacAlgorithm<TIn, TOut>
        where TIn : IAutoInputCoeff
        where TOut : IAutoOutputCoeff
    {
        /// <summary>
        /// Cài đặt khi load 
        /// </summary>
        void Initial(int tableNumber);

        /// <summary>
        /// Cập nhật hệ số
        /// </summary>
        /// <param name="coeff"></param>
        TOut UpdateCoeff(TIn coeff);

        /// <summary>
        /// Tính toán hệ số hiện tại dựa trên lịch sử lãi(lỗ) 
        /// </summary>
        /// <param name="historyProfits"></param>
        /// <returns></returns>
        int CalculateModCoeff(List<int> historyProfits, RootInputUpdateModel rootInputUpdate);

        /// <summary>
        /// Khi có shoe mới 
        /// </summary>
        void Reset();

        /// <summary>
        /// Thêm card vào database và threads, 
        /// đồng thời cập nhật cho session các cột liên quan
        /// </summary>
        /// <param name="baccrat"></param>
        void AddNewCard(BaccratCard baccrat, AutoResult autoResult = null);

        /// <summary>
        /// Dự đoán SAU KHI ĐÃ CÓ CARD
        /// </summary>
        /// <returns></returns>
        BaccaratPredict Predict();

        
        


        /// <summary>
        /// Các bước khi process 1 card 
        ///     1. TakeProfit.
        ///     2. Add new card.
        ///     3. Predict.
        ///     Các bước phải implement riêng rẽ để có thể gọi lại hàm TakeProfit() hoặc Predict() nhiều lần
        /// </summary>
        BaccaratPredict Process(BaccratCard baccratCard, AutoResult autoResult = null);

        /// <summary>
        /// Tạo session mới
        /// </summary>
        /// <returns></returns>
        AutoSession CreateNewSession();

        /// <summary>
        /// Cập nhật lại hệ số và lãi cho bước trước đó, không thêm card mới, không cập nhật database
        /// </summary>
        /// <param name="baccratCard"></param>
        System.Tuple<RootProfit<int>, RootProfit<int>> TakeProfit(BaccratCard newCard);

        /// <summary>
        /// Cập nhật lại thông tin cho autoRoot và autoSession sau khi thêm card mới
        /// </summary>
        /// <param name="profits"></param>
        void UpdateDatabase(Tuple<RootProfit<int>, RootProfit<int>> profits);
    }
}
