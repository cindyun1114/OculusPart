public class ProgressCalculator
{
    /// <summary>
    /// �p��@��@���i�ס]�C���`���Ƨ����^
    /// </summary>
    public static float CalculateOneToOneProgress(int completedChapters, int totalChapters)
    {
        if (totalChapters <= 0) return 0f;
        return (completedChapters / (float)totalChapters) * 100f;
    }

    /// <summary>
    /// �p��ЫǶi�ס]�䴩�C���`���P�v���^
    /// </summary>
    public static float CalculateClassroomProgress(int completedChapters, int totalChapters, float[] chapterWeights = null)
    {
        if (totalChapters <= 0) return 0f;

        if (chapterWeights == null || chapterWeights.Length != totalChapters)
        {
            // �S��weights�A�w�]����
            return (completedChapters / (float)totalChapters) * 100f;
        }
        else
        {
            // �p�G�C���`���Ƥ��@�ˡA�ھ�weights�Ӻ�
            float earned = 0f;
            float total = 0f;
            for (int i = 0; i < totalChapters; i++)
            {
                total += chapterWeights[i];
                if (i < completedChapters)
                {
                    earned += chapterWeights[i];
                }
            }
            return (earned / total) * 100f;
        }
    }

    /// <summary>
    /// ����i�� = �@��@+�ЫǦU50%
    /// </summary>
    public static float CalculateOverallProgress(float progressOneToOne, float progressClassroom)
    {
        return (progressOneToOne * 0.5f) + (progressClassroom * 0.5f);
    }
}