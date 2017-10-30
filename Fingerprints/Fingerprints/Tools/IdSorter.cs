using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Resources;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fingerprints.Tools
{
    /// <summary>
    /// Sorting DrawingServices Data Collections, to match same Ids on same rows
    /// </summary>
    class IdSorter
    {
        List<MinutiaStateBase> tmpData = null;
        List<MinutiaStateBase> tmpOppositeData = null;

        DrawingService drawingService = null;
        DrawingService oppositeDrawingService = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_firstService"></param>
        /// <param name="_secondService"></param>
        public IdSorter(DrawingService _firstService, DrawingService _secondService)
        {
            try
            {
                drawingService = _firstService;
                oppositeDrawingService = _secondService;
                tmpData = _firstService.DrawingData.ToList();
                tmpOppositeData = _secondService.DrawingData.ToList();

                               
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Sort lists
        /// </summary>
        public void SortById()
        {
            string id = String.Empty;
            string oppositeId = String.Empty;
            try
            {
                // If one list is empty, there is nothing to sort, therefore leave meothod 
                if (tmpData.Count == 0 || tmpOppositeData.Count == 0)
                    return;

                //Fill with empties to match list count
                FillEmpty();
                
                //match same features from both lists
                for (int indexFrom = 0; indexFrom < tmpOppositeData.Count; indexFrom++)
                {
                    //if Empty type, next loop
                    if (tmpOppositeData[indexFrom].Minutia.DrawingType == DrawingType.Empty)
                        continue;

                    // Get Id 
                    oppositeId = tmpOppositeData[indexFrom].Id;

                    //look for same id in both list
                    for (int indexTo = 0; indexTo < tmpData.Count(); indexTo++)
                    {
                        id = tmpData[indexTo].Id;

                        //swap elements on list to match id and break out of loop
                        if (oppositeId == id)
                        {
                            tmpData.Swap(indexFrom, indexTo);
                            break;
                        }

                        // if nothing found insert empty on indexFrom and add empty to the oppositeList to to preserve 
                        // both list count
                        if (indexTo == tmpData.Count - 1)
                        {
                            tmpData.Insert(indexFrom, new EmptyState(oppositeDrawingService));
                            tmpOppositeData.Add(new EmptyState(drawingService));
                            break;
                        }

                    }
                }

                // Cut unecessary empty object lines 
                CutUnecessaryEmpty();

                //Refresh ObservableCollections from DrawingServices
                RefreshDrawingServicesDataCollections();              

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Refresh Data collections 
        /// </summary>
        private void RefreshDrawingServicesDataCollections()
        {
            try
            {
                drawingService.DrawingData.Clear();
                drawingService.DrawingData.AddRange(tmpData);

                oppositeDrawingService.DrawingData.Clear();
                oppositeDrawingService.DrawingData.AddRange(tmpOppositeData);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Cut Empty type objects from list
        /// </summary>
        private void CutUnecessaryEmpty()
        {
            int count = 0;
            try
            {
                count = tmpData.Count();
                for (int index = 0; index < count; index++)
                {
                    if (tmpData[index].Minutia.DrawingType == DrawingType.Empty && tmpOppositeData[index].Minutia.DrawingType == DrawingType.Empty)
                    {
                        tmpData.RemoveAt(index);
                        tmpOppositeData.RemoveAt(index);
                        count--;
                        index--;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Fill empty to match list count
        /// </summary>
        private void FillEmpty()
        {
            try
            {
                if (tmpData.Count > tmpOppositeData.Count)
                    for (int i = 0; i < tmpData.Count - tmpOppositeData.Count + 1; i++)
                        tmpOppositeData.Add(new EmptyState(oppositeDrawingService));
                else
                    for (int i = 0; i < tmpOppositeData.Count - tmpData.Count + 1; i++)
                        tmpData.Add(new EmptyState(drawingService));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
